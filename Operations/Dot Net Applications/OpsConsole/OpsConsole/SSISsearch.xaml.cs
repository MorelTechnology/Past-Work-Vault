using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
// using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for SSISsearch.xaml
    /// </summary>
    public partial class SSISsearch : UserControl
    {
        public class envInfo
        {
            public string path = "";
            public string serverPackageFolderPath = "";
            public DataTable dtSteps = new DataTable();
            public DataTable dtJobs = new DataTable();
        }

        envInfo prod = new envInfo() { path = @"c:\packages-prod", serverPackageFolderPath = @"\\manprodbs01\c$" };
        envInfo test = new envInfo() { path = @"c:\packages-test", serverPackageFolderPath = @"\\mantestbs01\c$" };
        envInfo dev = new envInfo() { path = @"c:\packages-dev", serverPackageFolderPath = @"\\mandevbs01\c$" };

        bool loaded = false;
        DataTable dtProdROC = new DataTable();
        DataTable dtTestROC = new DataTable();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public enum environment { prod, test, dev };

        public class subresult
        {
            public string color { get; set; }
            public string SA { get; set; }
            public string jobid { get; set; }
            public string stepid { get; set; }
        }

        public class results
        {
            public string File { get; set; }
            public string FileColor { get; set; }
            public string Line { get; set; }
            public int LineNo { get; set; }

            public subresult dev { get; set; }
            public subresult prod { get; set; }
            public subresult test { get; set; }

            public string ROCProdJob { get; set; }
            public string ROCTestJob { get; set; }
        }
        List<results> resultList = new List<results>();
        int occurances = 0;

        public class jobInfo
        {
            public string Step { get; set; }
            public string StepWeight { get; set; }
            public string Type { get; set; }
            public string TypeWeight { get; set; }
            public string Description { get; set; }
            public string DescriptionWeight { get; set; }
            public string Command { get; set; }
            public string CommandWeight { get; set; }
        }

        public SSISsearch()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Start();
        }

        public void InitialLoad()
        {
            btnProdVsTest.Visibility = System.Windows.Visibility.Collapsed;
            btnProdVsDev.Visibility = System.Windows.Visibility.Collapsed;
            btnTestVsDev.Visibility = System.Windows.Visibility.Collapsed;

            btnOpenProdPackage.Visibility = System.Windows.Visibility.Collapsed;
            btnOpenTestPackage.Visibility = System.Windows.Visibility.Collapsed;
            btnOpenDevPackage.Visibility = System.Windows.Visibility.Collapsed;

            if (loaded)
                return;

            imgAngle.Angle = 0d;
            imgWait.Visibility = System.Windows.Visibility.Visible;
            FillProdTestDevComparison();
            StartWork();

            loaded = true;
 
        }

        private void ShowSSISandROC()
        {
            foreach (results r in resultList)
            {
                if (r.prod.color == "Green")
                    ShowSSISandROCspecifics(r, prod.dtJobs, prod.dtSteps, r.File, "Prod", r.prod);
                if (r.test.color != "White")
                    ShowSSISandROCspecifics(r, test.dtJobs, test.dtSteps, r.File, "Test", r.test);
                if (r.dev.color != "White")
                    ShowSSISandROCspecifics(r, dev.dtJobs, dev.dtSteps, r.File, "Dev", r.dev);
            }
        }

        private void ShowSSISandROCspecifics(results r, DataTable jobs, DataTable steps, string filename, string loc, subresult sr)
        {
            foreach (DataRow drStep in steps.Rows)
            {
                if (parseSSISjob(drStep["command"].ToString()).ToUpper() == filename.ToUpper())
                {
                    foreach (DataRow drJob in jobs.Rows)
                        if ((drStep["subsystem"].ToString() == "SSIS") && (drJob["job_id"].ToString() == drStep["job_id"].ToString()) )
                        {
                            sr.SA = drJob["name"].ToString();
                            sr.jobid = drJob["job_id"].ToString();
                            sr.stepid = drStep["step_id"].ToString();

                            foreach (DataRow drROC in dtProdROC.Rows)
                            {
                                if ((r.prod.SA != null) && (drROC["JobName"].ToString().ToUpper() == r.prod.SA.ToUpper()))
                                    r.ROCProdJob = drROC["Description"].ToString();
                            }
                            foreach (DataRow drROC in dtTestROC.Rows)
                            {
                                if ((r.test.SA != null) && (drROC["JobName"].ToString().ToUpper() == r.test.SA.ToUpper()))
                                    r.ROCTestJob = drROC["Description"].ToString();
                            }

                        }
                }
            }
            dgSearchResults.ItemsSource = resultList;
        }

        private string parseSSISjob(string command)
        {
            if (command.StartsWith("/SQL"))
            {
                string ssisname = command.Substring(7);
                int endquote = ssisname.IndexOf('"');
                if (endquote > 0)
                    ssisname = ssisname.Substring(0, endquote);
                return ssisname+".dtsx";
            }

            if (command.StartsWith("/FILE"))
            {
                int dtsx = command.IndexOf(".dtsx");
                if (dtsx > 0)
                {
                    command = command.Substring(0,dtsx + 5);
                    int lastBackslash = command.LastIndexOf('\\');
                    if (lastBackslash > 0)
                        command = command.Substring(lastBackslash + 1);
                    return command;
                }
            }
            return command;

        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            prod.dtSteps = fillTableFromSQL("ManProdBS01", "msdb", @"select job_id, step_name, step_id, subsystem, command, last_run_duration, last_run_date, last_run_time from msdb.dbo.sysjobsteps order by job_id, step_id");
            prod.dtJobs = fillTableFromSQL("ManProdBS01", "msdb", @"select job_id, name, date_created, date_modified, version_number from msdb.dbo.sysjobs");
            test.dtSteps = fillTableFromSQL("ManTestBS01", "msdb", @"select job_id, step_name, step_id,  subsystem, command, last_run_duration, last_run_date, last_run_time from msdb.dbo.sysjobsteps order by job_id, step_id");
            test.dtJobs = fillTableFromSQL("ManTestBS01", "msdb", @"select job_id, name, date_created, date_modified, version_number from msdb.dbo.sysjobs");
            dev.dtSteps = fillTableFromSQL("ManDevBS01", "msdb", @"select job_id, step_name, step_id,  subsystem, command, last_run_duration, last_run_date, last_run_time from msdb.dbo.sysjobsteps order by job_id, step_id");
            dev.dtJobs = fillTableFromSQL("ManDevBS01", "msdb", @"select job_id, name, date_created, date_modified, version_number from msdb.dbo.sysjobs");
            dtProdROC = fillTableFromSQL("ManProdBS01", "ROC", @"select * from ROC.dbo.process");
            dtTestROC = fillTableFromSQL("ManTestBS01", "ROC", @"select * from ROC.dbo.process");

            Dispatcher.BeginInvoke((Action)(() => ShowSSISandROC()));
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imgWait.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void StartWork()
        {
            //Show your wait dialog
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //string[] fileEntries = Directory.GetFiles(path);
            //foreach (string fileName in fileEntries)
            //    ProcessFile(fileName);

            //dgSearchResults.DataContext = resultList;
            occurances = 0;
            foreach (results r in resultList)
            {
                if ((bool)xbSearchProd.IsChecked)
                {
                    if (r.prod.color == "Green")
                        ProcessFile(r, System.IO.Path.Combine(prod.path, r.File));
                }
            }

            dgSearchResults.ItemsSource = new List<results>();
            dgSearchResults.ItemsSource = resultList;
            lblOccurances.Text = occurances.ToString() + " occurances";
        }

        private void FillProdTestDevComparison()
        {
            bool refresh = false;

            List<string> filesAll = new List<string>();
            List<string> filesProd = new List<string>();
            List<string> filesTest = new List<string>();
            List<string> filesDev = new List<string>();

            try
            {
                if (Directory.Exists(prod.path) == false)
                    { Directory.CreateDirectory(prod.path); refresh = true; }
                if (Directory.Exists(test.path) == false)
                    { Directory.CreateDirectory(test.path); refresh = true; }
                if (Directory.Exists(dev.path) == false)
                    { Directory.CreateDirectory(dev.path); refresh = true; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"The folders " + prod.path + ", " + test.path + ", " + dev.path + " did not exist and the attempt to create them failed." + Environment.NewLine + "The specific error is:" + Environment.NewLine + Environment.NewLine + ex.ToString());
                return;
            }

            if (refresh)
                refreshAll();

            // Find all the SSIS packages for each environment
            string[] filesProdFull = Directory.GetFiles(prod.path);
            string[] filesTestFull = Directory.GetFiles(test.path);
            string[] filesDevFull = Directory.GetFiles(dev.path);

            foreach (string s in filesProdFull)
                filesProd.Add(filename(s));
            foreach (string s in filesTestFull)
                filesTest.Add(filename(s));
            foreach (string s in filesDevFull)
                filesDev.Add(filename(s));

            // Create a sorted list of all the package names that exist in any environment
            foreach (string fileName in filesProd)
                filesAll.Add(fileName);
            foreach (string fileName in filesTest)
                if (!filesAll.Contains(fileName))
                    filesAll.Add(fileName);
            foreach (string fileName in filesDev)
                if (!filesAll.Contains(fileName))
                    filesAll.Add(fileName);
            filesAll.Sort();

            foreach (string fileName in filesAll)
            {
                string prodcolor = (filesProd.Contains(fileName)) ? "Green" : "White";
                string testcolor = (filesTest.Contains(fileName)) ? "Green" : "White";

                if ((prodcolor == "Green") && (testcolor == "Green"))
                    if (FileCompare(Path.Combine(prod.path, fileName), Path.Combine(test.path, fileName)) == false)
                        testcolor = "Red";

                string devcolor = (filesDev.Contains(fileName)) ? "Green" : "White";

                if (devcolor == "Green")
                {
                    if (testcolor != "White")
                    {
                        if (FileCompare(Path.Combine(test.path, fileName), Path.Combine(dev.path, fileName)) == false)
                            devcolor = "Red";
                    }

                    else if (prodcolor != "White")
                    {
                        if (FileCompare(Path.Combine(prod.path, fileName), Path.Combine(dev.path, fileName)) == false)
                            devcolor = "Red";
                    }

                }

                resultList.Add(new results() { File = fileName, prod = new subresult() { color = prodcolor }, test = new subresult() { color = testcolor }, dev = new subresult() { color = devcolor }, FileColor = "Transparent" });

            }
//            dgSearchResults.ItemsSource = resultList;

        }

        // This method accepts two strings the represent two files to 
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the 
        // files are not the same.
        private bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }

        private string filename(string fullpath)
        {
            return System.IO.Path.GetFileName(fullpath);
        }

        private void ProcessFile(results r, string filename)
        {
            string lookingFor = ((bool) (!xbCaseSensitive.IsChecked)) ? txtSearchText.Text.ToUpper() : txtSearchText.Text;

            string[] lines = System.IO.File.ReadAllLines(filename);
            int lineNo = 0;
            r.FileColor = "Transparent";
            foreach (string line in lines)
            {
                if (((bool)(xbCaseSensitive.IsChecked)) ? (line.IndexOf(lookingFor) >= 0) :
                    (line.ToUpper().IndexOf(lookingFor) >= 0))
                {
                    r.Line = line;
                    r.LineNo = lineNo;
                    r.FileColor = "Goldenrod";
                    occurances++;
                }
                lineNo++;
            }
        }

        private void dgSearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dgSearchResults.SelectedIndex < 0) || (dgSearchResults.SelectedItem.ToString() == "{NewItemPlaceholder}"))
                return;

            results r = ((results)dgSearchResults.SelectedItem);
            btnProdVsTest.Visibility = (r.prod.color == "Green" && r.test.color != "White") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            btnProdVsDev.Visibility = (r.prod.color == "Green" && r.dev.color != "White") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            btnTestVsDev.Visibility = (r.test.color != "White" && r.dev.color != "White") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            btnOpenProdPackage.Visibility = (r.prod.color == "Green") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            btnOpenTestPackage.Visibility = (r.test.color != "White") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            btnOpenDevPackage.Visibility = (r.dev.color != "White") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            if (r.FileColor == "Goldenrod")
            {
                string[] lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(prod.path, ((results)dgSearchResults.SelectedItem).File));
                int iLine = ((results)dgSearchResults.SelectedItem).LineNo;

                string total = "";
                for (int offset = -5; offset < 6; offset++)
                {
                    if (offset + iLine >= 0)
                        total += lines[offset + iLine] + Environment.NewLine;
                }

                var rtf = @"{\rtf1\ansi " + total + " }";

                string lookingFor = ((bool)(!xbCaseSensitive.IsChecked)) ? txtSearchText.Text.ToUpper() : txtSearchText.Text;
                int iStartsAt = (((bool)(xbCaseSensitive.IsChecked)) ? rtf.IndexOf(lookingFor) :
                                rtf.ToUpper().IndexOf(lookingFor));

                string preamble = rtf.Substring(0, iStartsAt);
                string match = rtf.Substring(iStartsAt, lookingFor.Length);
                string post = rtf.Substring(iStartsAt + lookingFor.Length);

                rtf = preamble + @"\b " + match + @"\b0 " + post;
                rtf = rtf.Replace(Environment.NewLine, @" \line");

                // rest
                rtf = rtf.Replace("&#xA;", @" \line");

                rtbDetails.SelectAll();
                rtbDetails.Selection.Text = "";

                //rtbDetails.Selection.
                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
                rtbDetails.Selection.Load(stream, DataFormats.Rtf);
            }
            else
            {
                rtbDetails.SelectAll();
                rtbDetails.Selection.Text = "";
            }

            if (rectProdDetails.Opacity > 0.5d)
                showSAjobDetails(environment.prod);
            if (rectTestDetails.Opacity > 0.5d)
                showSAjobDetails(environment.test);
            if (rectDevDetails.Opacity > 0.5d)
                showSAjobDetails(environment.dev);

        }

        private void btnOpenPackage_Click(object sender, RoutedEventArgs e)
        {
            //if (dgSearchResults.SelectedIndex < 0)
            //    return;

            //Process.Start(System.IO.Path.Combine(path, ((results)dgSearchResults.SelectedItem).File));
        }




        // TEMPORARY !!!!
        private static DataTable fillTableFromSQL(string server, string database, string sql)
        {
            DataTable table1 = new DataTable("");
            using (SqlConnection conn = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + database + ";Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    conn.Open();
                    adapt.Fill(table1);
                    conn.Close();
                }
            }
            return table1;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            imgAngle.Angle += 5d;
            if (imgAngle.Angle >= 355d)
                imgAngle.Angle = 0d;
        }

        private void btnProdVsTest_Click(object sender, RoutedEventArgs e)
        {
            if (dgSearchResults.SelectedIndex < 0)
                return;

            Process.Start(appFolder() + "\\Executables\\windiff.exe", prod.path + "\\" + ((results)dgSearchResults.SelectedItem).File + " " + test.path + "\\" + ((results)dgSearchResults.SelectedItem).File);
        }

        private void btnProdVsDev_Click(object sender, RoutedEventArgs e)
        {
            if (dgSearchResults.SelectedIndex < 0)
                return;

            Process.Start(appFolder() + "\\Executables\\windiff.exe", prod.path + "\\" + ((results)dgSearchResults.SelectedItem).File + " " + dev.path + "\\" + ((results)dgSearchResults.SelectedItem).File);
        }

        private void btnTestVsDev_Click(object sender, RoutedEventArgs e)
        {
            if (dgSearchResults.SelectedIndex < 0)
                return;

            Process.Start(appFolder() + "\\Executables\\windiff.exe", test.path + "\\" + ((results)dgSearchResults.SelectedItem).File + " " + dev.path + "\\" + ((results)dgSearchResults.SelectedItem).File);
        }

        private void getWindiffIfNeeded()
        {
            if (File.Exists(appFolder() + "\\Executables\\windiff.exe"))
                return;

            try 
	        {	        
	        }
	        catch (Exception)
	        {
	        }
        }

        private string appFolder()
        {
            string appFolder = System.Reflection.Assembly.GetExecutingAssembly().Location;
            int lastBackslash = appFolder.LastIndexOf("\\");
            appFolder = appFolder.Substring(0, lastBackslash);
            return appFolder;
        }

        private void btnOpenProdPackage_Click(object sender, RoutedEventArgs e)
        {
            if (dgSearchResults.SelectedIndex < 0)
                return;

            Process.Start(System.IO.Path.Combine(prod.path, ((results)dgSearchResults.SelectedItem).File));
        }

        private void btnOpenTestPackage_Click(object sender, RoutedEventArgs e)
        {
            if (dgSearchResults.SelectedIndex < 0)
                return;

            Process.Start(System.IO.Path.Combine(test.path, ((results)dgSearchResults.SelectedItem).File));
        }

        private void btnOpenDevPackage_Click(object sender, RoutedEventArgs e)
        {
            if (dgSearchResults.SelectedIndex < 0)
                return;

            Process.Start(System.IO.Path.Combine(dev.path, ((results)dgSearchResults.SelectedItem).File));
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            foreach (results r in resultList)
                r.FileColor = "Transparent";

            dgSearchResults.ItemsSource = new List<results>();
            dgSearchResults.ItemsSource = resultList;

            rtbDetails.SelectAll();
            rtbDetails.Selection.Text = "";
            txtSearchText.Text = "";
        }

        private void btnProdDetails_Click(object sender, RoutedEventArgs e)
        {
            showSAjobDetails(environment.prod);
        }

        private void btnTestDetails_Click(object sender, RoutedEventArgs e)
        {
            showSAjobDetails(environment.test);
        }

        private void btnDevDetails_Click(object sender, RoutedEventArgs e)
        {
            showSAjobDetails(environment.dev);
        }

        private void showSAjobDetails(environment env)
        {
            rectProdDetails.Opacity = (env == environment.prod) ? 1d : 0.29d;
            rectTestDetails.Opacity = (env == environment.test) ? 1d : 0.29d;
            rectDevDetails.Opacity = (env == environment.dev) ? 1d : 0.29d;


            if ((dgSearchResults.SelectedIndex < 0) || (dgSearchResults.SelectedItem.ToString() == "{NewItemPlaceholder}"))
            {
                // clear the SA job steps
                return;
            }
            results r = ((results)dgSearchResults.SelectedItem);
            subresult sr = null;
            envInfo inf = null;

            string jobname = "";

            if (env == environment.prod)
            {
                sr = r.prod;
                inf = prod;
                jobname = r.prod.SA;
            }

            if (env == environment.test)
            {
                sr = r.test;
                inf = test;
                jobname = r.test.SA;
            }

            if (env == environment.dev)
            {
                sr = r.dev;
                inf = dev;
                jobname = r.dev.SA;
            }

            List<jobInfo> ji = new List<jobInfo>();            

            foreach (DataRow drStep in inf.dtSteps.Rows)
            {
                if (drStep["job_id"].ToString() == sr.jobid)
                {
                    string fontweight = (drStep["step_id"].ToString() == sr.stepid) ? "Bold" : "Normal";
                    ji.Add(new jobInfo() { Step = drStep["step_id"].ToString(), Type = drStep["subsystem"].ToString(), Description = drStep["step_name"].ToString(), Command = drStep["command"].ToString(), CommandWeight = fontweight, DescriptionWeight = fontweight, StepWeight = fontweight, TypeWeight = fontweight });
                }
            }
            dgSASteps.ItemsSource = ji;

        }

        private void dgSASteps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rtbDetails.SelectAll();
            rtbDetails.Selection.Text = "";

            if ((dgSASteps.SelectedIndex < 0) || (dgSASteps.SelectedItem.ToString() == "{NewItemPlaceholder}"))
                return;

            jobInfo j = ((jobInfo)dgSASteps.SelectedItem);
            int cfiat = j.Command.IndexOf("/CONFIGFILE");
            if (cfiat < 0)
                return;
            int qa = j.Command.IndexOf("\"", cfiat);
            if (qa < 0)
                return;
            string file = j.Command.Substring(qa + 1);
            int qa2 = file.IndexOf("\"");
            if (qa2 < 0)
                return;
            file = file.Substring(0, qa2);
            file = file.ToUpper();

            if (rectProdDetails.Opacity > 0.5d)
                file = file.Replace(@"C:\", prod.serverPackageFolderPath+"\\");
            if (rectTestDetails.Opacity > 0.5d)
                file = file.Replace(@"C:\", test.serverPackageFolderPath + "\\");
            if (rectDevDetails.Opacity > 0.5d)
                file = file.Replace(@"C:\", dev.serverPackageFolderPath + "\\");

            string[] lines = System.IO.File.ReadAllLines(file);
            //int lineNo = 0;

            //r.FileColor = "Transparent";
            string total = "";
            foreach (string line in lines)
            {
            //    if (((bool)(xbCaseSensitive.IsChecked)) ? (line.IndexOf(lookingFor) >= 0) :
            //        (line.ToUpper().IndexOf(lookingFor) >= 0))
            //    {
            //        r.Line = line;
            //        r.LineNo = lineNo;
            //        r.FileColor = "Goldenrod";
            //        occurances++;
            //    }
                    total += line + Environment.NewLine;
            //    lineNo++;
            }

            var rtf = @"{\rtf1\ansi " + total + " }";
            rtf = total;

            string rtfHeader = @"{\rtf1\ansi\deff0";
            rtfHeader += @"{\colortbl;\red0\green0\blue0;\red255\green0\blue0;\red0\green128\blue0;\red0\green0\blue220;}";

            //string lookingFor = ((bool)(!xbCaseSensitive.IsChecked)) ? txtSearchText.Text.ToUpper() : txtSearchText.Text;
            //int iStartsAt = (((bool)(xbCaseSensitive.IsChecked)) ? rtf.IndexOf(lookingFor) :
            //                rtf.ToUpper().IndexOf(lookingFor));

            //string preamble = rtf.Substring(0, iStartsAt);
            //string match = rtf.Substring(iStartsAt, lookingFor.Length);
            //string post = rtf.Substring(iStartsAt + lookingFor.Length);

            //rtf = preamble + @"\b " + match + @"\b0 " + post;

            rtf = rtf.Replace(@"\", @"\\");
            rtf = rtf.Replace(Environment.NewLine, @" \line");

            // Property" Path=".Variables[User::ANSCon].Properties[Value]" ValueType="String
            rtf = rtf.Replace(".Variables[User::", ".Variables[User::" + @"\b\cf3 ");
            rtf = rtf.Replace("<ConfiguredValue>", "<ConfiguredValue>" + @"\b ");
            rtf = rtf.Replace("</ConfiguredValue>", @"\b0 " + "</ConfiguredValue>");
            rtf = rtf.Replace("].Properties[Value]", @"\b0\cf1 " + "].Properties[Value]");

            //Package.Connections[StarsCon].Properties[ConnectionString]" 
            rtf = rtf.Replace(".Connections[", ".Connections[" + @"\b\cf4 ");
            rtf = rtf.Replace("].Properties[", @"\b0\cf1 " + "].Properties[");


            rtbDetails.SelectAll();
            rtbDetails.Selection.Text = "";

            // string test2 = swap("Initial foo=\"foo\";Data Source=ProdSQL4;Initial Catalog=STARS;trash=yes,foo xxx yyy aaa Data Source=ProdSQL4;Initial Catalog", "Data Source=", ";");

            rtf = swap(rtf, "Data Source=", ";");
            rtf = swap(rtf, "Initial Catalog=", ";");


            rtf = rtfHeader + rtf + " }";

            //rtbDetails.Selection.
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
            rtbDetails.Selection.Load(stream, DataFormats.Rtf);
        }


        // "Initial foo=\"foo\";Data Source=ProdSQL4;Initial Catalog=STARS;trash=yes", "Data Source=", ";foo xxx yyy aaa Data Source=ProdSQL4;Initial Catalog"
        //  ^ pos=0
        // foundat 20
        // semicolon found at 40
        // string = string up till 20+"Data Source=".length (12) + \cf2 + 

        private string swap(string rtf, string startat, string endat)
        {
            int pos = 0;
            while (true)
            {
                int foundStart = rtf.IndexOf(startat, pos);
                if (foundStart < 0)
                    return rtf;

                int foundEnd = rtf.IndexOf(endat, foundStart + 1);
                if (foundEnd < 0)
                    return rtf;

                string start = rtf.Substring(0, foundStart + startat.Length);
                string middle = rtf.Substring(foundStart+startat.Length, (foundEnd-foundStart)-startat.Length);
                string end = rtf.Substring(foundEnd);

                rtf = start + "\\cf2" + middle + "\\cf1" + end;
                pos = foundEnd + 8;
            }
        }

        private void btnViewPackageConfig_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnRefreshLocal_Click(object sender, RoutedEventArgs e)
        {
            refreshAll();
        }

        private void refreshAll()
        {
            refreshLocal("Data Source=MANPRODBS01;Initial Catalog=MSDB;Integrated Security=True", prod.path, prod.serverPackageFolderPath);
            refreshLocal("Data Source=MANTESTBS01;Initial Catalog=MSDB;Integrated Security=True", test.path, test.serverPackageFolderPath);
            refreshLocal("Data Source=MANDEVBS01;Initial Catalog=MSDB;Integrated Security=True", dev.path, dev.serverPackageFolderPath);
            MessageBox.Show("Refresh complete");
        }

        private void refreshLocal(string dataSource, string local, string server)
        {
            using (SqlConnection conn = new SqlConnection(dataSource))
            {
                string sqlcomm = "select name, packagedata from msdb.dbo.sysssispackages";

                FileStream stream;
                BinaryWriter writer;

                int bufferSize = 100;
                byte[] outByte = new byte[bufferSize];
                long retval;
                long startIndex = 0;


                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        //string path = "c:\\packages-test";
                        string filename = reader["name"].ToString();

                        // Create a file to hold the output.
                        stream = new FileStream(Path.Combine(local, filename) + ".dtsx", FileMode.OpenOrCreate, FileAccess.Write);
                        writer = new BinaryWriter(stream);

                        // Reset the starting byte for the new BLOB.
                        startIndex = 0;

                        // Read bytes into outByte[] and retain the number of bytes returned.
                        retval = reader.GetBytes(reader.GetOrdinal("packagedata"), startIndex, outByte, 0, bufferSize);

                        // Continue while there are bytes beyond the size of the buffer.
                        while (retval == bufferSize)
                        {
                            writer.Write(outByte);
                            writer.Flush();
                            startIndex += bufferSize;
                            retval = reader.GetBytes(reader.GetOrdinal("packagedata"), startIndex, outByte, 0, bufferSize);
                        }

                        // Write the remaining buffer.
                        if (retval > 0)
                            writer.Write(outByte, 0, (int)retval);
                        writer.Flush();
                        writer.Close();
                        stream.Close();
                    }
                }

            }
        }



    }
}

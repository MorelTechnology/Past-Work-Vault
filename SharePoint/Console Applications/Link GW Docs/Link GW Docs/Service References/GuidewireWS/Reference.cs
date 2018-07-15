﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Link_GW_Docs.GuidewireWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://example.com/riverstone/webservice/document/trg_UpdateMetadataAPI", ConfigurationName="GuidewireWS.trg_UpdateMetadataAPIPortType")]
    public interface trg_UpdateMetadataAPIPortType {
        
        // CODEGEN: Generating message contract since message updateMetadata has headers
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Link_GW_Docs.GuidewireWS.updateMetadataResponse updateMetadata(Link_GW_Docs.GuidewireWS.updateMetadata request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Link_GW_Docs.GuidewireWS.updateMetadataResponse> updateMetadataAsync(Link_GW_Docs.GuidewireWS.updateMetadata request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://guidewire.com/ws/soapheaders")]
    public partial class authentication : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string usernameField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
                this.RaisePropertyChanged("username");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
                this.RaisePropertyChanged("password");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://example.com/riverstone/webservice/document")]
    public partial class trg_MetadataPair : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string keyField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
                this.RaisePropertyChanged("key");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://example.com/riverstone/webservice/document")]
    public partial class trg_DocumentInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string documentIDField;
        
        private trg_MetadataPair[] metadataField;
        
        private int operationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string documentID {
            get {
                return this.documentIDField;
            }
            set {
                this.documentIDField = value;
                this.RaisePropertyChanged("documentID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Entry")]
        public trg_MetadataPair[] metadata {
            get {
                return this.metadataField;
            }
            set {
                this.metadataField = value;
                this.RaisePropertyChanged("metadata");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int operation {
            get {
                return this.operationField;
            }
            set {
                this.operationField = value;
                this.RaisePropertyChanged("operation");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1087.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://guidewire.com/ws/soapheaders")]
    public partial class locale : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
                this.RaisePropertyChanged("Value");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateMetadata", WrapperNamespace="http://example.com/riverstone/webservice/document/trg_UpdateMetadataAPI", IsWrapped=true)]
    public partial class updateMetadata {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://guidewire.com/ws/soapheaders")]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Link_GW_Docs.GuidewireWS.authentication authentication;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://guidewire.com/ws/soapheaders")]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Link_GW_Docs.GuidewireWS.locale locale;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://example.com/riverstone/webservice/document/trg_UpdateMetadataAPI", Order=0)]
        public Link_GW_Docs.GuidewireWS.trg_DocumentInfo metadata;
        
        public updateMetadata() {
        }
        
        public updateMetadata(Link_GW_Docs.GuidewireWS.authentication authentication, Link_GW_Docs.GuidewireWS.locale locale, Link_GW_Docs.GuidewireWS.trg_DocumentInfo metadata) {
            this.authentication = authentication;
            this.locale = locale;
            this.metadata = metadata;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateMetadataResponse", WrapperNamespace="http://example.com/riverstone/webservice/document/trg_UpdateMetadataAPI", IsWrapped=true)]
    public partial class updateMetadataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://example.com/riverstone/webservice/document/trg_UpdateMetadataAPI", Order=0)]
        public bool @return;
        
        public updateMetadataResponse() {
        }
        
        public updateMetadataResponse(bool @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface trg_UpdateMetadataAPIPortTypeChannel : Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class trg_UpdateMetadataAPIPortTypeClient : System.ServiceModel.ClientBase<Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType>, Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType {
        
        public trg_UpdateMetadataAPIPortTypeClient() {
        }
        
        public trg_UpdateMetadataAPIPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public trg_UpdateMetadataAPIPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public trg_UpdateMetadataAPIPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public trg_UpdateMetadataAPIPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Link_GW_Docs.GuidewireWS.updateMetadataResponse Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType.updateMetadata(Link_GW_Docs.GuidewireWS.updateMetadata request) {
            return base.Channel.updateMetadata(request);
        }
        
        public bool updateMetadata(Link_GW_Docs.GuidewireWS.authentication authentication, Link_GW_Docs.GuidewireWS.locale locale, Link_GW_Docs.GuidewireWS.trg_DocumentInfo metadata) {
            Link_GW_Docs.GuidewireWS.updateMetadata inValue = new Link_GW_Docs.GuidewireWS.updateMetadata();
            inValue.authentication = authentication;
            inValue.locale = locale;
            inValue.metadata = metadata;
            Link_GW_Docs.GuidewireWS.updateMetadataResponse retVal = ((Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType)(this)).updateMetadata(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Link_GW_Docs.GuidewireWS.updateMetadataResponse> Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType.updateMetadataAsync(Link_GW_Docs.GuidewireWS.updateMetadata request) {
            return base.Channel.updateMetadataAsync(request);
        }
        
        public System.Threading.Tasks.Task<Link_GW_Docs.GuidewireWS.updateMetadataResponse> updateMetadataAsync(Link_GW_Docs.GuidewireWS.authentication authentication, Link_GW_Docs.GuidewireWS.locale locale, Link_GW_Docs.GuidewireWS.trg_DocumentInfo metadata) {
            Link_GW_Docs.GuidewireWS.updateMetadata inValue = new Link_GW_Docs.GuidewireWS.updateMetadata();
            inValue.authentication = authentication;
            inValue.locale = locale;
            inValue.metadata = metadata;
            return ((Link_GW_Docs.GuidewireWS.trg_UpdateMetadataAPIPortType)(this)).updateMetadataAsync(inValue);
        }
    }
}
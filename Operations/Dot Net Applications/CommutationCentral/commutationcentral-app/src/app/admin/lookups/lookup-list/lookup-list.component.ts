import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupTypeSchema } from '../../../api/lookup-type-schema';
import { LookupsService } from '../../../api/services/lookups.service';
import { MenuItem, LazyLoadEvent, ConfirmationService } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import 'rxjs/add/operator/finally';
import { Lookup } from '../../../api/models/lookup';
import { MessageService } from 'primeng/components/common/messageservice';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-lookup-list',
  templateUrl: './lookup-list.component.html',
  styleUrls: ['./lookup-list.component.scss']
})
export class LookupListComponent implements OnInit {
  @ViewChild(Table) table: Table;
  public loadingLookups: boolean = true;
  public lookups: Lookup[];
  public tableColumns: any[];
  public pageSize: number;
  public totalCount: number;
  public lookupTypeSchema: LookupTypeSchema;

  private breadcrumb: MenuItem[];

  constructor(private route: ActivatedRoute, private lookupsService: LookupsService,
    private breadcrumbService: BreadcrumbService, private confirmationService: ConfirmationService,
    private router: Router, private messageService: MessageService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      //TODO error handle param doesn't have type described
      if (params.has("type")) {
        this.lookupTypeSchema = this.lookupsService.getTypeSchema(params.get("type"));
        this.breadcrumb = [
          { label: "Administration", routerLink: "/admin", routerLinkActiveOptions: { exact: true } },
          { label: "Manage Lookups", routerLink: "/admin/lookups", routerLinkActiveOptions: { exact: true } },
          { label: this.lookupTypeSchema.PluralName, routerLink: ["/admin/lookups", this.lookupTypeSchema.InternalName] }
        ];
        this.breadcrumbService.setBreadcrumbs(this.breadcrumb);
        this.setTableColumns();
        //this.getAllLookups();
      }
    });
  }

  private setTableColumns(): void {
    this.tableColumns = [
      { field: 'Id', header: 'Id' },
      { field: 'Name', header: 'Name' }
    ];
  }

  private getAllLookups(queryString?: string): void {
    this.loadingLookups = true;
    this.lookupsService.getAll(this.lookupTypeSchema.InternalName, queryString).finally(() => {
      this.loadingLookups = false;
    }).subscribe(lookups => {
      this.lookups = <Lookup[]>lookups.body;
      let paginationData = JSON.parse(lookups.headers.get("X-Pagination"));
      this.pageSize = paginationData.pageSize;
      this.totalCount = paginationData.totalCount;
    });
  }

  public loadLookupsLazy(event: LazyLoadEvent) {
    let queryString: string = '';
    //TODO Filters
    if (event.rows) {
      if (queryString !== '') queryString += '&';
      queryString += `PageSize=${event.rows}`;
    }
    if (event.first && event.rows) {
      if (queryString !== '') queryString += '&';
      let page = Math.ceil((event.first + 1) / event.rows);
      queryString += `PageNumber=${page}`;
    }
    if (event.sortField) {
      if (queryString !== '') queryString += '&';
      queryString += `OrderBy=${event.sortField}`;
      if (event.sortOrder) {
        queryString += ` ${event.sortOrder === 1 ? 'asc' : 'desc'}`;
      }
    }

    this.getAllLookups(queryString === '' ? null : queryString);
  }

  public confirmDeleteLookup(lookup: Lookup) {
    this.confirmationService.confirm({
      message: `Are you sure you want to remove the ${this.lookupTypeSchema.SingularName} '${lookup.Name}'?`,
      accept: () => {
        this.lookupsService.delete(this.lookupTypeSchema.InternalName, lookup.Id).subscribe(() => {
          this.messageService.add({
            severity: 'success',
            summary: `Successfully Removed ${this.lookupTypeSchema.SingularName}`,
            detail: `Removed ${this.lookupTypeSchema.SingularName} ${lookup.Name}`
          });
          let loadEvent: LazyLoadEvent = {
            rows: this.table.rows,
            first: this.table.first,
            sortField: this.table.sortField,
            sortOrder: this.table.sortOrder
          };
          this.loadLookupsLazy(loadEvent);
        });
      }
    })
  }

  public editLookup(lookup: Lookup) {
    this.router.navigate(['/admin/lookups', this.lookupTypeSchema.InternalName, lookup.Id]);
  }

  public addLookup() {
    this.router.navigate(['/admin/lookups', this.lookupTypeSchema.InternalName, 'new']);
  }

}

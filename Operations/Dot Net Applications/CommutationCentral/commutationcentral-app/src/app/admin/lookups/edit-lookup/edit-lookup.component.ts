import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupTypeSchema } from '../../../api/lookup-type-schema';
import { LookupsService } from '../../../api/services/lookups.service';
import { Lookup } from '../../../api/models/lookup';
import { MenuItem } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-edit-lookup',
  templateUrl: './edit-lookup.component.html',
  styleUrls: ['./edit-lookup.component.scss']
})
export class EditLookupComponent implements OnInit {
  public pageTitle: string;
  public lookupForm: FormGroup;
  public lookupFormSubmitted: boolean = false;
  public lookup: Lookup;

  private lookupTypeSchema: LookupTypeSchema;
  private breadcrumb: MenuItem[];

  constructor(private route: ActivatedRoute, private lookupsService: LookupsService,
    private breadcrumbService: BreadcrumbService, private fb: FormBuilder, private router: Router,
    private messageService: MessageService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      //TODO handle param doesn't have type
      if (params.has("type")) {
        //TODO handle no matching type schema
        this.lookupTypeSchema = this.lookupsService.getTypeSchema(params.get("type"));
        // TODO handle param type doesn't have id
        if (params.has("id")) {
          let lookupId = +params.get("id");
          this.lookupsService.get(this.lookupTypeSchema.InternalName, lookupId).subscribe(response => {
            this.lookup = <Lookup>response.body;
            this.pageTitle = `Editing ${this.lookupTypeSchema.SingularName} - ${this.lookup.Name}`;
            this.breadcrumb = [
              { label: 'Administration', routerLink: '/admin', routerLinkActiveOptions: { exact: true } },
              { label: 'Manage Lookups', routerLink: '/admin/lookups', routerLinkActiveOptions: { exact: true } },
              {
                label: this.lookupTypeSchema.PluralName,
                routerLink: ['/admin/lookups', this.lookupTypeSchema.InternalName],
                routerLinkActiveOptions: { exact: true }
              },
              {
                label: this.lookup.Name,
                routerLink: ['/admin/lookups', this.lookupTypeSchema.InternalName, this.lookup.Id],
                routerLinkActiveOptions: { exact: true }
              }
            ];
            this.breadcrumbService.setBreadcrumbs(this.breadcrumb);
            this.buildLookupForm();
          } /*TODO handle no matching lookup*/)
        }
      }
    });
  }

  private buildLookupForm() {
    this.lookupForm = this.fb.group({
      Name: new FormControl(this.lookup.Name, [Validators.required])
    });
  }

  private prepareSubmit(): Lookup {
    return <Lookup>this.lookupForm.value;
  }

  public cancelEdit(): void {
    this.router.navigate(['/admin/lookups', this.lookupTypeSchema.InternalName]);
  }

  public submit(): void {
    this.lookupFormSubmitted = true;
    let modifiedLookup = this.prepareSubmit();
    this.lookupsService.update(this.lookupTypeSchema.InternalName, modifiedLookup, this.lookup.Id).finally(() => {
      this.lookupFormSubmitted = false;
    }).subscribe(() => {
      this.messageService.add({
        severity: 'success',
        summary: `Successfully Updated ${this.lookupTypeSchema.SingularName}`,
        detail: `Edited ${this.lookupTypeSchema.SingularName} ${modifiedLookup.Name}`
      });
      this.router.navigate(['/admin/lookups', this.lookupTypeSchema.InternalName]);
    });
  }

}

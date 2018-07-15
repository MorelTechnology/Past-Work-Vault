import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { LookupTypeSchema } from '../../../api/lookup-type-schema';
import { MenuItem } from 'primeng/components/common/api';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupsService } from '../../../api/services/lookups.service';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { Lookup } from '../../../api/models/lookup';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-new-lookup',
  templateUrl: './new-lookup.component.html',
  styleUrls: ['./new-lookup.component.scss']
})
export class NewLookupComponent implements OnInit {
  public pageTitle: string;
  public lookupForm: FormGroup;
  public lookupFormSubmitted: boolean = false;

  private lookupTypeSchema: LookupTypeSchema;
  private breadcrumb: MenuItem[];

  constructor(private route: ActivatedRoute, private lookupsService: LookupsService,
    private breadcrumbService: BreadcrumbService, private fb: FormBuilder, private router: Router,
    private messageService: MessageService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      // TODO handle param doesn't have type
      if (params.has("type")) {
        //TODO handle no matching type schema
        this.lookupTypeSchema = this.lookupsService.getTypeSchema(params.get("type"));
        this.pageTitle = `Creating New ${this.lookupTypeSchema.SingularName}`;
        this.breadcrumb = [
          { label: 'Administration', routerLink: '/admin', routerLinkActiveOptions: { exact: true } },
          { label: 'Manage Lookups', routerLink: '/admin/lookups', routerLinkActiveOptions: { exact: true } },
          {
            label: this.lookupTypeSchema.PluralName,
            routerLink: ['/admin/lookups', this.lookupTypeSchema.InternalName],
            routerLinkActiveOptions: { exact: true }
          },
          {
            label: `New ${this.lookupTypeSchema.SingularName}`,
            routerLink: ['/admin/lookups', this.lookupTypeSchema.InternalName, 'new'],
            routerLinkActiveOptions: { exact: true }
          }
        ];
        this.breadcrumbService.setBreadcrumbs(this.breadcrumb);
        this.buildLookupForm();
      }
    })
  }

  private buildLookupForm() {
    this.lookupForm = this.fb.group({
      Name: new FormControl("", [Validators.required])
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
    let newLookup = this.prepareSubmit();
    this.lookupsService.create(this.lookupTypeSchema.InternalName, newLookup).finally(() => {
      this.lookupFormSubmitted = false;
    }).subscribe(response => {
      let createdLookup = <Lookup>response.body;
      this.messageService.add({
        severity: 'success',
        summary: `Successfully Created ${this.lookupTypeSchema.SingularName}`,
        detail: `Created ${this.lookupTypeSchema.SingularName} ${createdLookup.Name}`
      });
      this.router.navigate(['/admin/lookups', this.lookupTypeSchema.InternalName]);
    });
  }

}

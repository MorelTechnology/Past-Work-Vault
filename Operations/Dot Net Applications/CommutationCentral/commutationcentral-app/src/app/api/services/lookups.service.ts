import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from "../../../environments/environment";
import { Lookup } from '../models/lookup';
import { LookupTypeSchema } from '../lookup-type-schema';
import { BaseApiService } from './base-api.service';

@Injectable()
export class LookupsService {
  private baseEndpoint: string = environment.api_url;
  private typeSchemas: LookupTypeSchema[] = [
    new LookupTypeSchema("ActivityCategories", "Activity Categories", "Activity Category"),
    new LookupTypeSchema("ActivityDroppedReasons", "Activity Dropped Reasons", "Activity Dropped Reason"),
    new LookupTypeSchema("ActivityPriorities", "Activity Priorities", "Activity Priority"),
    new LookupTypeSchema("ActivityStatuses", "Activity Statuses", "Activity Status"),
    new LookupTypeSchema("CommutationStatuses", "Commutation Statuses", "Commutation Status"),
    new LookupTypeSchema("CommutationTypes", "Commutation Types", "Commutation Type"),
    new LookupTypeSchema("Companies", "Companies", "Company"),
    new LookupTypeSchema("CompanyStatuses", "Company Statuses", "Company Status"),
    new LookupTypeSchema("DealChecklistItems", "Deal Checklist Items", "Deal Checklist Item"),
    new LookupTypeSchema("DealPriorities", "Deal Priorities", "Deal Priority"),
    new LookupTypeSchema("DroppedReasons", "Dropped Reasons", "Dropped Reason"),
    new LookupTypeSchema("FairfaxEntities", "Fairfax Entities", "Fairfax Entity"),
    new LookupTypeSchema("LeadOffices", "Lead Offices", "Lead Office"),
    new LookupTypeSchema("NoteEntryTypes", "Note Entry Types", "Note Entry Type"),
    new LookupTypeSchema("RequestTypes", "Request Types", "Request Type")
  ];

  constructor(private http: HttpClient, private baseApiService: BaseApiService) { }

  public getTypeSchema(typeInternalName: string): LookupTypeSchema {
    return this.typeSchemas.find(schema => schema.InternalName === typeInternalName);
  }

  public getAll(entityType: string, queryString?: string) {
    let url = this.baseEndpoint + entityType;
    if (queryString) url += `?${queryString}`;
    return this.baseApiService.get(url);
  }

  public get(entityType: string, id: number) {
    let url = `${this.baseEndpoint}${entityType}/${id}`;
    return this.baseApiService.get(url);
  }

  public update(entityType: string, lookup: Lookup, lookupId: number) {
    let url = `${this.baseEndpoint}${entityType}/${lookupId}`;
    return this.baseApiService.put(url, lookup);
  }

  public create(entityType: string, lookup: Lookup) {
    let url = `${this.baseEndpoint}${entityType}`;
    return this.baseApiService.post(url, lookup);
  }

  public delete(entityType: string, id: number) {
    let url = `${this.baseEndpoint}${entityType}/${id}`;
    return this.baseApiService.delete(url);
  }
}

import { Injectable, EventEmitter } from '@angular/core';
import { MenuItem } from "primeng/components/common/api";

@Injectable()
export class BreadcrumbService {
  private items: MenuItem[] = [];

  constructor() { }

  public breadcrumbChanged: EventEmitter<MenuItem[]> = new EventEmitter<MenuItem[]>();

  public getBreadcrumb(): MenuItem[] {
    return this.items;
  }

  public setBreadcrumb(breadcrumb: MenuItem[]): void {
    this.items = breadcrumb;
    this.breadcrumbChanged.emit(this.items);
  }

}

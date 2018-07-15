import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { MenuItem } from 'primeng/components/common/api';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class BreadcrumbService {
  private breadCrumbs: Subject<MenuItem[]>;
  breadcrumbsChanged$: Observable<MenuItem[]>;

  constructor() {
    this.breadCrumbs = new Subject<MenuItem[]>();
    this.breadcrumbsChanged$ = this.breadCrumbs.asObservable();
  }

  public setBreadcrumbs(items: MenuItem[]) {
    this.breadCrumbs.next(items);
  }

}

import { TestBed, async, inject } from '@angular/core/testing';

import { AppAdminGuard } from './app-admin.guard';

describe('AppAdminGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppAdminGuard]
    });
  });

  it('should ...', inject([AppAdminGuard], (guard: AppAdminGuard) => {
    expect(guard).toBeTruthy();
  }));
});

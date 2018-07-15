import { TestBed, async, inject } from '@angular/core/testing';

import { NoAccessDeniedGuard } from './no-access-denied.guard';

describe('NoAccessDeniedGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NoAccessDeniedGuard]
    });
  });

  it('should ...', inject([NoAccessDeniedGuard], (guard: NoAccessDeniedGuard) => {
    expect(guard).toBeTruthy();
  }));
});

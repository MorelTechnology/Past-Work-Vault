import { TestBed, async, inject } from '@angular/core/testing';

import { AppUserGuard } from './app-user.guard';

describe('AppUserGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppUserGuard]
    });
  });

  it('should ...', inject([AppUserGuard], (guard: AppUserGuard) => {
    expect(guard).toBeTruthy();
  }));
});

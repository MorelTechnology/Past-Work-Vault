import { TestBed, inject } from '@angular/core/testing';

import { WorkRequestsService } from './work-requests.service';

describe('WorkRequestsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WorkRequestsService]
    });
  });

  it('should be created', inject([WorkRequestsService], (service: WorkRequestsService) => {
    expect(service).toBeTruthy();
  }));
});

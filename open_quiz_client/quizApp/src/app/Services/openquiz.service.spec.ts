import { TestBed } from '@angular/core/testing';

import { OpenquizService } from './openquiz.service';

describe('OpenquizService', () => {
  let service: OpenquizService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OpenquizService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

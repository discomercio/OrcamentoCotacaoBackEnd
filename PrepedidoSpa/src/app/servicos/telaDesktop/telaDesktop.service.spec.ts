/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TelaDesktopService } from './telaDesktop.service';

describe('Service: TelaDesktop', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TelaDesktopService]
    });
  });

  it('should ...', inject([TelaDesktopService], (service: TelaDesktopService) => {
    expect(service).toBeTruthy();
  }));
});

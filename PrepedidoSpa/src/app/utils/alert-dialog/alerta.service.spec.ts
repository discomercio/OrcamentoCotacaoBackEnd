/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AlertaService } from './alerta.service';

describe('Service: Alerta', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AlertaService]
    });
  });

  it('should ...', inject([AlertaService], (service: AlertaService) => {
    expect(service).toBeTruthy();
  }));
});

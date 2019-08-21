/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PrepedidoBuscaService } from './prepedido-busca.service';

describe('Service: PrepedidoBusca', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PrepedidoBuscaService]
    });
  });

  it('should ...', inject([PrepedidoBuscaService], (service: PrepedidoBuscaService) => {
    expect(service).toBeTruthy();
  }));
});

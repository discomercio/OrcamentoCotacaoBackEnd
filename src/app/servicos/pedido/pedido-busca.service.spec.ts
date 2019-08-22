/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PedidoBuscaService } from './pedido-busca.service';

describe('Service: PedidoBusca', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PedidoBuscaService]
    });
  });

  it('should ...', inject([PedidoBuscaService], (service: PedidoBuscaService) => {
    expect(service).toBeTruthy();
  }));
});

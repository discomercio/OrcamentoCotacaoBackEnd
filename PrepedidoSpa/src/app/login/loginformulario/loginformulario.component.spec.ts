import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginformularioComponent } from './loginformulario.component';

describe('FormularioComponent', () => {
  let component: LoginformularioComponent;
  let fixture: ComponentFixture<LoginformularioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginformularioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginformularioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

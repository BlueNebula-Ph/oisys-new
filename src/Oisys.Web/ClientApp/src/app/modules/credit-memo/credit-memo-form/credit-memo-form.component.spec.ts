import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditMemoFormComponent } from './credit-memo-form.component';

describe('CreditMemoFormComponent', () => {
  let component: CreditMemoFormComponent;
  let fixture: ComponentFixture<CreditMemoFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreditMemoFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreditMemoFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

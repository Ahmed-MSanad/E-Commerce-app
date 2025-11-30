import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthLayotComponent } from './auth-layot.component';

describe('AuthLayotComponent', () => {
  let component: AuthLayotComponent;
  let fixture: ComponentFixture<AuthLayotComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthLayotComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthLayotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

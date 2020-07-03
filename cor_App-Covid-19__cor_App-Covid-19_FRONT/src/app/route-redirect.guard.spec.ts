import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ConfigTestingModule } from '@commons';
import { RouteRedirectGuard } from './route-redirect.guard';

describe('RouteRedirectGuard', () => {
  let guard: RouteRedirectGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, ConfigTestingModule, RouterTestingModule]
    });
    guard = TestBed.inject(RouteRedirectGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});

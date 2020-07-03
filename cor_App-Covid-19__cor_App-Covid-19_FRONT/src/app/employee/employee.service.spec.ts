import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ConfigTestingModule } from '@commons';
import { EmployeeService } from './employee.service';

describe('EmployeeService', () => {
  let service: EmployeeService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, ConfigTestingModule]
    });
    service = TestBed.inject(EmployeeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

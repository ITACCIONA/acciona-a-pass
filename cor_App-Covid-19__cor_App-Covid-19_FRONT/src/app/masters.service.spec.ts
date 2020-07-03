import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ConfigTestingModule } from '@commons';
import { MastersService } from './masters.service';

const mockUser = roles => ({
  idEmpleado: null,
  nombreUsuario: null,
  inicialesUsuario: null,
  roleName: roles
});

describe('MastersService', () => {
  let service: MastersService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, ConfigTestingModule]
    });
    service = TestBed.inject(MastersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('roles', () => {
    it('medical services should work', () => {
      service.userInfo = mockUser(['Empleado', 'ServicioMedico']);
      expect(service.getRole()).toBe('ServicioMedico');
    });

    it('HR role should work', () => {
      service.userInfo = mockUser(['Empleado', 'RRHHDesc']);
      expect(service.getRole()).toBe('RRHHDesc');
    });

    it('Communications should work', () => {
      service.userInfo = mockUser(['Empleado', 'Comunicacion']);
      expect(service.getRole()).toBe('Comunicacion');
    });

    it('Occupational risk prevention role should work', () => {
      service.userInfo = mockUser(['Empleado', 'PRL']);
      expect(service.getRole()).toBe('PRL');
    });
  });
});

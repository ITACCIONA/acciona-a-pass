import { TestBed } from '@angular/core/testing';
import { ConfigModule } from './config.module';
import { ConfigService } from './config.service';

jest.mock('../../../environments/environment');

describe('ConfigService', () => {
  let service: ConfigService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ConfigModule]
    });
    service = TestBed.get(ConfigService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('retrieve existent single property', () => {
    expect(service.get('hello')).toEqual('world');
  });

  it('retrieve existent multiple property', () => {
    expect(service.get('goodbye.world')).toEqual('today');
  });
  it('retrieve undefined single property', () => {
    expect(service.get('none')).toBe(undefined);
  });

  it('retrieve undefined multiple property', () => {
    expect(service.get('hello.none')).toBe(undefined);
  });

  it('retrieve url single defined property', () => {
    expect(service.getUrl('i18n')).toBe('/aval/api/i18n/');
  });
  it('retrieve url single merged defined property', () => {
    expect(service.getUrl('testing')).toBe('/api/testing/');
  });

  it('retrieve undefined single undefined property', () => {
    expect(service.getUrl('hello')).toBe(undefined);
  });

  it('retrieve undefined multiple undefined property', () => {
    expect(service.getUrl('hello.none')).toBe(undefined);
  });
});

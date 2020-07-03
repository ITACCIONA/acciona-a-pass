export interface IEmployee {
  idEmpleado: number;
  nombreEmpleado: string;
  inicialesEmpleado: string;
  edadEmpleado: number;
  numEmpleado: number;
  departamento: string;
  nameLocalizacion: string;
  pais: string;
  direccion1: string;
  ciudad: string;
  codigoPostal: string;
  division: string;
  estadoPasaporte: string;
  colorPasaporte: string;
  accion: string;
  fechaCreacion: string;
  fechaExpiracion: string;
  estadoDatabaseId: number;
}

export interface IEmployeeListSingle {
  idEmpleado: number;
  nombre: string;
  genero: string;
  numEmpleado: number;
  edad: number;
  ultiModifi: string;
  estado: string;
  colorPasaporte: string;
}

export interface IEmployeeList {
  employees: IEmployeeListSingle[];
  page: number;
  numElements: number;
  elementsPerPage: number;
}

export interface IEmployeePersonal {
  apellidosEmpleado: string;
  ciudad: string;
  codigoPostal: string;
  departamento: string;
  direccion1: string;
  division: string;
  dni: string;
  edadEmpleado: number;
  fechaNacimiento: string;
  generoEmpleado: string;
  idEmpleado: number;
  inicialesEmpleado: string;
  mailEmpleado: string;
  nameLocalizacion: string;
  nombreEmpleado: string;
  numEmpleado: number;
  pais: string;
  responsable: string;
  telefonoEmpleado: string;
}

export interface IRiskFactors {
  fechaFactor: string;
  idRiskFactor: number;
  name: string;
  value: boolean;
}

export interface IResponsibleStatement<T = any[]> {
  date: string;
  values: T;
}

export interface IResponsibleStatementResp {
  symptomsByDay: IResponsibleStatement[];
}

export interface IRiskFactorsResp {
  valoracionFactorRiesgos: IRiskFactors[];
}

export interface ITests {
  testRapidos: {
    id: number;
    fechaTest: string;
    control: boolean;
    igg: boolean;
    igm: boolean;
  }[];
  testPCR: {
    id: number;
    fechaTest: string;
    positivo: boolean;
  }[];
}

export interface IMedicalMonitoring {
  nameParameterType: 'Analitica' | 'Imagen';
  monitoringValue: {
    id: number;
    fechaTest: string;
    comment: string;
    parameterValues: {
      nameParameter: string;
      value: boolean;
    }[];
  }[];
}
export interface IMedicalMonitoringResp {
  medicalMonitoring: IMedicalMonitoring[];
}

export interface ITemperature {
  value: boolean;
  date: string;
}

export interface ITemperatureResp {
  meditions: ITemperature[];
}

export interface GenericResponse<T = any> {
  info: any;
  data: T[];
  success: boolean;
}

import { Router } from '@angular/router';
import { I18NextPipe } from 'angular-i18next';
import { ITableHeaders } from '../../../ui/table/table.component';

export interface IComplementary {
  sortField?: string;
}

export const EMPLOYEES_LIST_HEADERS_DEFAULT: (router: Router) => Array<ITableHeaders & IComplementary> = (
  router: Router
) => [
  { field: 'numEmpleado', sortField: 'NumEmpleado', label: `EMPLOYEE.TABLE.ID` },
  {
    field: 'nombre',
    sortField: 'Nombre',
    label: `EMPLOYEE.TABLE.NAME`,
    click: row => router.navigate(['/employee', row.idEmpleado])
  }
];

export const EMPLOYEES_LIST_HEADERS_MEDICAL: (router: Router) => Array<ITableHeaders & IComplementary> = (
  router: Router
) => [
  ...EMPLOYEES_LIST_HEADERS_DEFAULT(router),
  { field: 'edad', sortField: 'Edad', label: `EMPLOYEE.TABLE.AGE` },
  { field: 'genero', sortField: 'Genero', label: `EMPLOYEE.TABLE.GENDER` },
  { field: 'ultiModifi', sortField: 'UltimaModif', label: `EMPLOYEE.TABLE.LAST_MOD` },
  {
    label: `EMPLOYEE.TABLE.STATUS`,
    field: 'estado',
    sortField: 'Estado',
    render: row => `<span class="dot ${row.colorPasaporte}"></span> ${row.estado}`
  }
];

export const EMPLOYEES_LIST_HEADERS_PRL: (
  router: Router,
  i18n: I18NextPipe
) => Array<ITableHeaders & IComplementary> = (router: Router, i18n: I18NextPipe) => [
  ...EMPLOYEES_LIST_HEADERS_DEFAULT(router),
  {
    label: `EMPLOYEE.TABLE.COLOR`,
    field: 'colorTxt',
    sortField: 'Estado',
    render: row => `<span class="dot ${row.colorPasaporte}"></span> ${i18n.transform(row.colorTxt)}`
  }
];

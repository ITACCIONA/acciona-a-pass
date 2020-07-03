import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { I18NextPipe } from 'angular-i18next';
import { Subscription } from 'rxjs';
import { IEmployeeListSingle } from '../../employee/interfaces';

export interface ITableHeaders {
  field: string;
  label: string;
  render?: (row: any) => string;
  click?: (row: any) => void;
}

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent implements AfterViewInit, OnDestroy, OnInit {
  @ViewChild(MatPaginator, { static: true })
  private paginator: MatPaginator;

  @ViewChild(MatSort) sort: MatSort;

  @Input() data: IEmployeeListSingle[];
  @Input() tableDef: ITableHeaders[];

  @Output() sortEvent = new EventEmitter<Array<string>>();
  @Output() pageChange = new EventEmitter<any>();

  @Input() totalResults = 0;
  @Input() pageSize: number;

  @Input() set page(page: number) {
    this.paginator.pageIndex = page - 1;
  }

  private sub$: Subscription;

  constructor(private i18n: I18NextPipe) {}

  ngOnInit() {
    this.translatePaginator();
  }

  ngAfterViewInit() {
    this.sub$ = this.sort.sortChange.subscribe(() => {
      this.sortEvent.emit([this.sort.active, this.sort.direction]);
    });
  }

  get displayedColumns() {
    return this.tableDef.map(h => h.field);
  }

  ngOnDestroy() {
    this.sub$.unsubscribe();
  }

  private translatePaginator() {
    this.paginator._intl.firstPageLabel = this.i18n.transform('PAGINATOR.FIRSTPAGE');
    this.paginator._intl.lastPageLabel = this.i18n.transform('PAGINATOR.LASTPAGE');
    this.paginator._intl.nextPageLabel = this.i18n.transform('PAGINATOR.NEXTPAGE');
    this.paginator._intl.previousPageLabel = this.i18n.transform('PAGINATOR.PREVIOUSPAGE');
    this.paginator._intl.getRangeLabel = () => {
      const page = this.paginator.pageIndex;

      if (this.totalResults === 0 || this.pageSize === 0) {
        return `${this.totalResults} ${this.i18n.transform('PAGINATOR.RESULTS')}`;
      }

      const startIndex = page * this.pageSize;

      const endIndex =
        startIndex < this.totalResults
          ? Math.min(startIndex + this.pageSize, this.totalResults)
          : startIndex + this.pageSize;

      return `${startIndex + 1} - ${endIndex} ${this.i18n.transform('PAGINATOR.SEPARATOR')} ${
        this.totalResults
      } ${this.i18n.transform('PAGINATOR.RESULTS')}`;
    };
  }

  emitPageChange(event: any) {
    this.pageChange.emit(event);
  }
}

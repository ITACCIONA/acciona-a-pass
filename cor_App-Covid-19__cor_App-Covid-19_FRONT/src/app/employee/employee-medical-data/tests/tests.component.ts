import { Component, Input, OnInit } from '@angular/core';
import { ITestResult } from '../../../ui/test-result/test-result.component';
import { EmployeeService } from '../../employee.service';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.scss']
})
export class TestsComponent implements OnInit {
  @Input()
  id: string;

  isLoading = true;

  tests: ITestResult[] = [];

  constructor(private service: EmployeeService) {}

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.isLoading = true;
    this.service.getTests(this.id).subscribe(r => {
      this.isLoading = false;
      const rapid = r.testRapidos.map(t => ({
        id: t.id,
        date: t.fechaTest,
        name: 'TESTS.RAPID',
        positive: (t.igg && t.igm) || (t.igm && !t.igg),
        breakdown: [
          { name: 'TESTS.CONTROL', positive: t.control },
          { name: t.igg ? 'TESTS.IGG_POSITIVE' : 'TESTS.IGG_NEGATIVE', positive: t.igg },
          { name: t.igm ? 'TESTS.IGM_POSITIVE' : 'TESTS.IGM_NEGATIVE', positive: t.igm }
        ]
      }));
      const pcr = r.testPCR.map(t => ({
        id: t.id,
        date: t.fechaTest,
        name: 'TESTS.PCR',
        positive: t.positivo,
        breakdown: [{ name: t.positivo ? 'TESTS.POSITIVE' : 'TESTS.NEGATIVE', positive: t.positivo }]
      }));
      this.tests = [...rapid, ...pcr];
      this.tests.sort((a, b) => (new Date(b.date) as any) - (new Date(a.date) as any));
    });
  }
}

import { Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';

import { EmployeeService } from 'src/app/_services/employee.service';
Chart.register(...registerables);
@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css'],
})
export class PieChartComponent {
  totalTimeMap = new Map<string, number>();
  @ViewChild('pieChartCanvas') pieChartCanvas!: ElementRef;
  constructor(public employeeService: EmployeeService) {
    employeeService.getTotalTimeWorked().subscribe((totalTimeMap) => {
      this.totalTimeMap = totalTimeMap;
      this.renderPieChart();
    });
  }

  renderPieChart() {
    const canvas = this.pieChartCanvas.nativeElement;
    const ctx = canvas.getContext('2d');

    const labels = Array.from(this.totalTimeMap.keys());
    const data = Array.from(this.totalTimeMap.values());

    const chart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: labels,
        datasets: [
          {
            data: data,
            backgroundColor: [
              '#FF6384',
              '#36A2EB',
              '#FFCE56',
              '#7C4DFF',
              '#4BC0C0',
              '#FF9F40',
              '#800080',
              '#FFD700',
            ],
          },
        ],
      },
      options: {
        responsive: true,
      },
    });
  }
}

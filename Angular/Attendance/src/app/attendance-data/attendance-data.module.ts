import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AttendanceTableComponent } from './attendance-table/attendance-table.component';
import { PieChartComponent } from './pie-chart/pie-chart.component';

@NgModule({
  declarations: [AttendanceTableComponent, PieChartComponent],
  imports: [CommonModule, HttpClientModule],
  exports: [AttendanceTableComponent, PieChartComponent],
})
export class AttendanceDataModule {}

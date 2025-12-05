import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-hr-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, TagModule],
  templateUrl: './hr-attendance.html',
  styleUrl: './hr-attendance.css'
})
export class HrAttendanceComponent {
  // TODO: This component currently uses static data as there is no backend API for Attendance yet.
  attendance = [
    { id: 1, employee: 'Dr. John Smith', date: '2024-01-15', checkIn: '8:00 AM', checkOut: '5:00 PM', status: 'Present' },
    { id: 2, employee: 'Mary Williams', date: '2024-01-15', checkIn: '7:45 AM', checkOut: '4:30 PM', status: 'Present' }
  ];
}


import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DatePickerModule } from 'primeng/datepicker';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-appointments-calendar',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, DatePickerModule, ButtonModule, CardModule, TableModule],
  templateUrl: './appointments-calendar.html',
  styleUrl: './appointments-calendar.css'
})
export class AppointmentsCalendarComponent {
  selectedDate: Date = new Date();
  
  events = [
    {
      title: 'John Doe - General Checkup',
      date: '2024-01-15',
      time: '10:00 AM',
      doctor: 'Dr. Smith'
    },
    {
      title: 'Jane Smith - Cardiology',
      date: '2024-01-16',
      time: '2:00 PM',
      doctor: 'Dr. Johnson'
    },
    {
      title: 'Michael Johnson - Orthopedics',
      date: '2024-01-17',
      time: '9:00 AM',
      doctor: 'Dr. Williams'
    }
  ];
}


import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DatePickerModule } from 'primeng/datepicker';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { AppointmentsService } from '../services/appointments.service';

@Component({
  selector: 'app-appointments-calendar',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, DatePickerModule, ButtonModule, CardModule, TableModule],
  templateUrl: './appointments-calendar.html',
  styleUrl: './appointments-calendar.css'
})
export class AppointmentsCalendarComponent implements OnInit {
  private appointmentsService = inject(AppointmentsService);

  selectedDate: Date = new Date();
  allAppointments: any[] = [];
  events: any[] = [];
  appointmentDates: Set<string> = new Set();

  ngOnInit() {
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentsService.getAll({ pageSize: 50 }).subscribe({
      next: (response) => {
        this.allAppointments = response.data.map(appt => {
          const apptDate = new Date(appt.appointmentDateTime);
          this.appointmentDates.add(apptDate.toDateString());
          return {
            title: `${appt.patientName} - ${appt.serviceName}`,
            date: apptDate.toLocaleDateString(),
            time: apptDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
            doctor: appt.doctorName,
            rawDate: apptDate
          };
        });
        this.filterEvents();
      },
      error: (error) => {
        console.error('Error loading appointments', error);
      }
    });
  }

  filterEvents() {
    if (!this.selectedDate) {
      this.events = [...this.allAppointments];
      return;
    }

    const selectedDateStr = this.selectedDate.toLocaleDateString();
    this.events = this.allAppointments.filter(event => event.date === selectedDateStr);
  }

  hasAppointment(date: any): boolean {
    const d = new Date(date.year, date.month, date.day);
    return this.appointmentDates.has(d.toDateString());
  }
}


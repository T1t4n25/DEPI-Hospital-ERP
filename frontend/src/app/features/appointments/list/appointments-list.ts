import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-appointments-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './appointments-list.html',
  styleUrl: './appointments-list.css'
})
export class AppointmentsListComponent {
  appointments = [
    { id: 1, patient: 'John Doe', doctor: 'Dr. Smith', service: 'General Checkup', date: '2024-01-15', time: '10:00 AM', status: 'Scheduled' },
    { id: 2, patient: 'Jane Smith', doctor: 'Dr. Johnson', service: 'Cardiology', date: '2024-01-16', time: '2:00 PM', status: 'Completed' },
    { id: 3, patient: 'Michael Johnson', doctor: 'Dr. Williams', service: 'Orthopedics', date: '2024-01-17', time: '9:00 AM', status: 'Cancelled' }
  ];

  searchTerm = '';

  get filteredAppointments() {
    if (!this.searchTerm) return this.appointments;
    const term = this.searchTerm.toLowerCase();
    return this.appointments.filter(a => 
      a.patient.toLowerCase().includes(term) ||
      a.doctor.toLowerCase().includes(term) ||
      a.service.toLowerCase().includes(term)
    );
  }

  getStatusSeverity(status: string) {
    switch(status) {
      case 'Scheduled': return 'info';
      case 'Completed': return 'success';
      case 'Cancelled': return 'danger';
      default: return 'warn';
    }
  }
}


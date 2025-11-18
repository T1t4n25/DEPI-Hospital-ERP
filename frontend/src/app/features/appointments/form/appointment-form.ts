import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-appointment-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CardModule, ButtonModule, DatePickerModule, SelectModule, TextareaModule, RippleModule],
  templateUrl: './appointment-form.html',
  styleUrl: './appointment-form.css'
})
export class AppointmentFormComponent {
  appointment = {
    patient: null as any,
    doctor: null as any,
    service: null as any,
    date: null as Date | null,
    time: '',
    notes: ''
  };

  patients = [
    { label: 'John Doe', value: 1 },
    { label: 'Jane Smith', value: 2 },
    { label: 'Michael Johnson', value: 3 }
  ];

  doctors = [
    { label: 'Dr. Smith', value: 1 },
    { label: 'Dr. Johnson', value: 2 },
    { label: 'Dr. Williams', value: 3 }
  ];

  services = [
    { label: 'General Checkup', value: 1 },
    { label: 'Cardiology', value: 2 },
    { label: 'Orthopedics', value: 3 },
    { label: 'Neurology', value: 4 }
  ];

  timeSlots = [
    '9:00 AM', '9:30 AM', '10:00 AM', '10:30 AM', '11:00 AM', '11:30 AM',
    '12:00 PM', '12:30 PM', '1:00 PM', '1:30 PM', '2:00 PM', '2:30 PM',
    '3:00 PM', '3:30 PM', '4:00 PM', '4:30 PM', '5:00 PM'
  ].map(time => ({ label: time, value: time }));

  onSubmit() {
    console.log('Form submitted:', this.appointment);
  }
}


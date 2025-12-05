import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { MedicalRecordsService } from '../services/medical-records.service';
import { PatientsService } from '../../patients/services/patients.service';
import { CreateMedicalRecordModel } from '../models/create-medical-record.model';
import { UpdateMedicalRecordModel } from '../models/update-medical-record.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-medical-record-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    CardModule,
    ButtonModule,
    DatePickerModule,
    SelectModule,
    RippleModule,
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './medical-record-form.html',
  styleUrl: './medical-record-form.css'
})
export class MedicalRecordFormComponent implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private medicalRecordsService = inject(MedicalRecordsService);
  private patientsService = inject(PatientsService);
  private http = inject(HttpClient);
  private messageService = inject(MessageService);

  isSubmitting = false;
  isEditMode = false;
  isLoading = false;
  recordId: number | null = null;

  record = {
    patientID: null as number | null,
    doctorID: null as number | null,
    diagnosesid: null as number | null,
    diagnoseDate: null as Date | null
  };

  patients: any[] = [];
  doctors: any[] = [];
  diagnoses: any[] = [];

  ngOnInit() {
    this.loadDropdownData();

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.recordId = +params['id'];
        this.loadRecordData();
      }
    });
  }

  loadDropdownData() {
    // Load patients
    this.patientsService.getAll({ pageSize: 1000 }).subscribe({
      next: (response) => {
        this.patients = response.data.map((p: any) => ({
          label: `${p.firstName} ${p.lastName}`,
          value: p.patientID
        }));
      },
      error: (err) => console.error('Error loading patients:', err)
    });

    // Load doctors (employees with Doctor role)
    this.http.get<any>(`${environment.apiUrl}/api/employees`, {
      params: { pageSize: '1000' }
    }).subscribe({
      next: (response) => {
        this.doctors = response.data
          .filter((e: any) => e.roleName === 'Doctor')
          .map((e: any) => ({
            label: `Dr. ${e.firstName} ${e.lastName}`,
            value: e.employeeID
          }));
      },
      error: (err) => console.error('Error loading doctors:', err)
    });

    // Load diagnoses
    this.http.get<any>(`${environment.apiUrl}/api/diagnoses`, {
      params: { pageSize: '1000' }
    }).subscribe({
      next: (response) => {
        this.diagnoses = response.data.map((d: any) => ({
          label: d.diagnoses,
          value: d.diagnosesID
        }));
      },
      error: (err) => console.error('Error loading diagnoses:', err)
    });
  }

  loadRecordData() {
    if (!this.recordId) return;

    this.isLoading = true;
    this.medicalRecordsService.getById(this.recordId).subscribe({
      next: (data) => {
        this.record = {
          patientID: data.patientID,
          doctorID: data.doctorID,
          diagnosesid: data.diagnosesid,
          diagnoseDate: new Date(data.diagnoseDate)
        };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading medical record:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load medical record data' });
        this.isLoading = false;
      }
    });
  }

  onSubmit() {
    if (this.isSubmitting) return;

    // Validation
    if (!this.record.patientID || !this.record.doctorID || !this.record.diagnosesid || !this.record.diagnoseDate) {
      this.messageService.add({ severity: 'warn', summary: 'Validation', detail: 'Please fill in all required fields.' });
      return;
    }

    this.isSubmitting = true;

    // Format date to YYYY-MM-DD
    const formattedDate = this.record.diagnoseDate ?
      new Date(this.record.diagnoseDate).toISOString().split('T')[0] : '';

    if (this.isEditMode && this.recordId) {
      // Update existing record
      const model: UpdateMedicalRecordModel = {
        recordID: this.recordId,
        patientID: this.record.patientID,
        doctorID: this.record.doctorID,
        diagnosesid: this.record.diagnosesid,
        diagnoseDate: formattedDate
      };

      this.medicalRecordsService.update(this.recordId, model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Medical record updated successfully' });
          setTimeout(() => this.router.navigate(['/medical-records']), 1000);
        },
        error: (error) => {
          console.error('Error updating medical record:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to update medical record' });
          this.isSubmitting = false;
        }
      });
    } else {
      // Create new record
      const model: CreateMedicalRecordModel = {
        patientID: this.record.patientID,
        doctorID: this.record.doctorID,
        diagnosesid: this.record.diagnosesid,
        diagnoseDate: formattedDate
      };

      this.medicalRecordsService.create(model).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Medical record created successfully' });
          setTimeout(() => this.router.navigate(['/medical-records']), 1000);
        },
        error: (error) => {
          console.error('Error creating medical record:', error);
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || 'Failed to create medical record' });
          this.isSubmitting = false;
        }
      });
    }
  }
}

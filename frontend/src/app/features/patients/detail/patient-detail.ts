import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { PatientsService } from '../services/patients.service';
import { PatientDetailModel } from '../models/patient-detail.model';

@Component({
  selector: 'app-patient-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    CardModule,
    ButtonModule,
    TagModule,
    RippleModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './patient-detail.html',
  styleUrl: './patient-detail.css'
})
export class PatientDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private patientsService = inject(PatientsService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  patient: PatientDetailModel | null = null;
  isLoading = true;
  patientId: number = 0;

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.patientId = +params['id'];
      this.loadPatientData();
    });
  }

  loadPatientData() {
    this.isLoading = true;
    this.patientsService.getById(this.patientId).subscribe({
      next: (data) => {
        this.patient = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading patient:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load patient data'
        });
        this.isLoading = false;
      }
    });
  }

  editPatient() {
    this.router.navigate(['/patients', this.patientId, 'edit']);
  }

  deletePatient() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this patient? This action cannot be undone.',
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => {
        this.patientsService.delete(this.patientId).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Patient deleted successfully'
            });
            setTimeout(() => this.router.navigate(['/patients']), 1000);
          },
          error: (error) => {
            console.error('Error deleting patient:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: error.message || 'Failed to delete patient'
            });
          }
        });
      }
    });
  }
}

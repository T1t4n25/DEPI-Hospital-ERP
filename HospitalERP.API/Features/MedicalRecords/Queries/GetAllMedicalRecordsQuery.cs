using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using MediatR;

namespace HospitalERP.API.Features.MedicalRecords.Queries;

public record GetAllMedicalRecordsQuery(QueryParams QueryParams) : IRequest<PaginatedResponse<MedicalRecordListDto>>;


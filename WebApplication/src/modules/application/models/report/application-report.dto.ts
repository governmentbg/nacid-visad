import { ReportType } from "../../enums/report-type.enum";
import { ApplicationReportItemDto } from "./application-report-item.dto";

export class ApplicationReportDto {
	reportType: ReportType;
	reports: ApplicationReportItemDto[] = [];
}

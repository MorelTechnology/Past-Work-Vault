import { WorkRequestStatus, BusinessValueUnit, RealizationOfImpact } from "../enums";

export class WorkRequest {
    RequestID: number = 0;
    Requestor: string;
    RequestorDisplayName: string;
    Manager: string;
    ManagerDisplayName: string;
    Title: string;
    Goal: string;
    NonImplementImpact: string;
    BusinessValueUnit: BusinessValueUnit;
    BusinessValueAmount: number;
    StatusDate: Date;
    Status: WorkRequestStatus;
    LastModified: Date;
    CorporateGoals: string[];
    GoalSupport: string;
    SupportsDept: boolean;
    DeptGoalSupport: string;
    ConditionsOfSatisfaction: string;
    RequestedCompletionDate: Date;
    RealizationOfImpact: RealizationOfImpact;
}

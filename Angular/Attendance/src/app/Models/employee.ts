export class Employee {
  constructor(
    public Id: string,
    public EmployeeName: string,
    public StarTimeUtc: Date,
    public EndTimeUtc: Date,
    public EntryNotes: string,
    public TotalTimeWorked: number
  ) {}
}

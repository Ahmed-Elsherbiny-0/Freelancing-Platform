export interface Job {
  id: number;
  title: string;
  description: string;
  budget: number;
  deadline: string;
  reqiuiredSkills: string[];
  createdDate: string;
  status: string;
  photoUrl: string;
  fristName: string;
  lastName: string;
  email: string;
}
export interface JobRequestDto {
  title: string;
  description: string;
  budget: number;
  requiredSkills: string[] | null;
  duration: string;
}

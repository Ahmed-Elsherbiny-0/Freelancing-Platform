export interface Offer {
  description: string;
  duration: string;
  price: number;
  isApproved: boolean;
  isCompleted: boolean;
  jobId: number;
  rating: number;
  completedTasks: number;
  pictureUrl: string;
  fristName: string;
  lastName: string;
  workerEamil: string;
  jobTitle?: string;
  workerId?: string;
}

export type offerDto = {
  jobId: number;
  description: string;
  Price: number;
  Duration: string;
};

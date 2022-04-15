export interface Project {
  key: string;
  displayName?: string;
  name?: string;
  avatar?: string;
  description?: string;
  terms?: string[];
  translationCollections?: string[];
  status?: number;
  accessLevel?: number;
  hide?: boolean;
}

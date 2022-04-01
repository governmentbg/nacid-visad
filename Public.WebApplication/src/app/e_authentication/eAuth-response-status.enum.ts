export enum EAuthResponseStatus {
  CanceledByUser = 1,
  AuthenticationFailed = 2,
  InvalidResponseXML = 3,
  InvalidSignature = 4,
  MissingEGN = 5,
  NotDetectedQES = 6,
  Success = 7
}

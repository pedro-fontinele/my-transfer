export enum EXmlHttpRequestReadyState {
    Unset = 0, // open() não foi chamado ainda.
    Opened = 1, // send() não foi chamado ainda.
    Headers_Received = 2, // send() foi chamado, e cabeçalhos e status estão disponíveis.
    Loading = 3, // responseText contém dados parciais.
    Done = 4, // A operação está concluída.
  }
  
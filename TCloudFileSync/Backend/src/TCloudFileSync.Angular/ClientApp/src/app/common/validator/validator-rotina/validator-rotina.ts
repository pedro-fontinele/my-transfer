import { Rotina } from "src/app/domain/entity/rotina/rotina";

export abstract class ValidatorRotina {
    public static validaRotinaFluxoServidor(rotina: Rotina): Rotina {
         if (rotina.ativo == 'true' || rotina.ativo == true) {
            rotina.ativo = true;
         } else {
           rotina.ativo = false;
         }
         
         if (rotina.deveApagarArquivoOrigem == 'true' || rotina.deveApagarArquivoOrigem == true) {
           rotina.deveApagarArquivoOrigem = true;
         } else {
           rotina.deveApagarArquivoOrigem = false;
         }
         
         if (rotina.fluxoLocalParaNuvem == 'true' || rotina.fluxoLocalParaNuvem == true) {
             rotina.fluxoLocalParaNuvem = true;
         } else {
             rotina.fluxoLocalParaNuvem = false;
         }

         return rotina;
    }

    public static validaRotinaFluxoAplicacao(rotina: Rotina): Rotina {
        if (rotina.ativo == true) {
           rotina.ativo = 'true';
        } else {
          rotina.ativo = 'false';
        }
        
        if (rotina.deveApagarArquivoOrigem == true) {
          rotina.deveApagarArquivoOrigem = 'true';
        } else {
          rotina.deveApagarArquivoOrigem = 'false';
        }
        
        if (rotina.fluxoLocalParaNuvem == true) {
            rotina.fluxoLocalParaNuvem = 'true';
        } else {
            rotina.fluxoLocalParaNuvem = 'false';
        }

        return rotina;
   }
}
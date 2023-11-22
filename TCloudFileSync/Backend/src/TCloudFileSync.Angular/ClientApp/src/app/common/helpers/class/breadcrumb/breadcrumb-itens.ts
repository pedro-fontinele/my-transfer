import { PoBreadcrumb } from "@po-ui/ng-components";

export abstract class PoBreadcrumbItens{
    public static poBreadcrumbSettings: PoBreadcrumb = {
        items: [
            { label: 'Início', link: '/' }, 
            { label: 'Configurações' }
        ]
    }

    public static poBreadcrumbHistoric: PoBreadcrumb = {
        items: [
            { label: 'Início', link: '/' }, 
            { label: 'Histórico' }
        ]
    }
}
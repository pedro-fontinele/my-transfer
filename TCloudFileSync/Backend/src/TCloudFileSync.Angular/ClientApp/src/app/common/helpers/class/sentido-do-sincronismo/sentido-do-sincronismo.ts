import { PoCheckboxGroupOption, PoSelectOption } from "@po-ui/ng-components";

export abstract class SentidoDoSincronismo {
    public static sentidoDoSincronismo: Array<any> = [
            { label: "Do local para nuvem", value: 'true' },
            { label: "Da nuvem para local", value: 'false' }
    ]
}
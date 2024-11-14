const vscode = require("vscode")
const BGEMode = {scheme: "file", language: "bge"}

class HoverProvider{
    provideHover(document, position, token){
        let wordRange = document.getWordRangeAtPosition(position, /[\w:]+/)
        if(!wordRange) return Promise.reject("none")

        let currentWord = document.lineAt(position.line).text.slice(wordRange.start.character, wordRange.end.character);
        let text = currentWord
        if( document.lineAt(position.line).text[0] != ":" &&
            document.lineAt(position.line).text[0] != "/"){
            return Promise.reject("comment")
        }
        if((/^([\da-fA-F]){1,4}$/).test(currentWord)){
            text = "0x"+currentWord+"("+parseInt(currentWord,16)+")"
        }else if(currentWord && currentWord[0] == ":"){
            text = "jump tag: "+currentWord.substring(1)
        }
        return Promise.resolve(new vscode.Hover(text))
    }
}
class CompletionItemProvider{
    provideCompletionItems(document, position, token){
        if(document.lineAt(position.line).text[0] != "/"){
            return Promise.resolve(new vscode.CompletionList([],false))
        }
        return Promise.resolve(new vscode.CompletionList([
            {label: "pop", kind: vscode.CompletionItemKind.Method},
            {label: "cls", kind: vscode.CompletionItemKind.Method},
            {label: "add", kind: vscode.CompletionItemKind.Method},
            {label: "sub", kind: vscode.CompletionItemKind.Method},
            {label: "mul", kind: vscode.CompletionItemKind.Method},
            {label: "div", kind: vscode.CompletionItemKind.Method},
            {label: "rem", kind: vscode.CompletionItemKind.Method},
            {label: "nand", kind: vscode.CompletionItemKind.Method},
            {label: "equal", kind: vscode.CompletionItemKind.Method},
            {label: "greater", kind: vscode.CompletionItemKind.Method},
            {label: "truejump", kind: vscode.CompletionItemKind.Method},
            {label: "jump", kind: vscode.CompletionItemKind.Method},
            {label: "call", kind: vscode.CompletionItemKind.Method},
            {label: "ret", kind: vscode.CompletionItemKind.Method},
            {label: "load", kind: vscode.CompletionItemKind.Method},
            {label: "store", kind: vscode.CompletionItemKind.Method},
            {label: "dumpkey", kind: vscode.CompletionItemKind.Method},
            {label: "redraw", kind: vscode.CompletionItemKind.Method},
            {label: "rect", kind: vscode.CompletionItemKind.Method},
            {label: "graph", kind: vscode.CompletionItemKind.Method},
            {label: "sound", kind: vscode.CompletionItemKind.Method},
            {label: "io", kind: vscode.CompletionItemKind.Method},
        ],false))
    }
}
function activate(context){
    console.log("we")
    context.subscriptions.push(vscode.languages.registerHoverProvider(BGEMode, new HoverProvider()))
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(BGEMode, new CompletionItemProvider(), 'Space'));
}
module.exports = {activate}
grammar BMM;
file: (root)* EOF;

root: extern | function;
statement: var ';'
     | call ';'
     | subs ';'
     | if |
     while;
expr: '(' expr ')'
     | opr
     | call
     | NUMBER
     | WORD;
opr: mul
     | div
     | rem
     | add
     | sub;

funcarg: WORD (',' WORD)*;
code: (statement)*;

extern: 'extern' WORD ';';

function: 'func' WORD '(' funcarg ')' '{' code '}';

if:       'if' '(' expr ')' '{' code '}';
while: 'while' '(' expr ')' '{' code '}';

var: 'var' WORD;
subs: WORD '=' expr;
call: WORD '(' expr (',' expr)* ')';

add: expr '+' expr;
sub: expr '-' expr;
mul: expr '*' expr;
div: expr '/' expr;
rem: expr '%' expr;


WORD: [a-zA-Z_]+;
NUMBER: [0-9]+;
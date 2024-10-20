import re
from lark import Lark
from sys import argv as arg

rule = open('bmm.lark').read()
parser = Lark(rule, start='module', parser='lalr')
reInclude = re.compile(r"^\s*#include\s+.+")
reComment = re.compile(r"^\s*//")

def compile(path):
    print("--Compile " + path + "...")
    program = open(path).read()

    print("Parsing...")
    cleancode = ""
    for item in program.split('\n'):
        if(reInclude.match(item)):
            pass
        elif(reComment.match(item)):
            pass
        else:
            cleancode += item + "\n"
    tree = parser.parse(cleancode)
    
    print("Tokenizing...")
    root = tree.children
    for item in root:
        print(item)

if __name__ == '__main__':
    compile(arg[1])
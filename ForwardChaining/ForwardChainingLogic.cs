using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForwardChaining
{
    class ForwardChainingLogic
    {
        List<Fact> oldFacts = new List<Fact>();
        List<Fact> newFacts = new List<Fact>();
        List<Fact> facts = new List<Fact>();
        List<Rule> rules = new List<Rule>();
        List<Rule> usedRules = new List<Rule>();
        Fact result;
        int iteration = 1;
        enum State { Default, Rule, Fact, Objective}
        public enum RuleState {Default,UseRaiseFlag1,SkipNoFact,SkipRaiseFlag2,Flag1,Flag2}
        RuleState state = RuleState.Default;
        bool progress = true;
        bool completed = false;



        public void Start()
        {
            ParseInput("input3.txt");
            PrintData();
            Console.WriteLine("2 DALIS. Vykdymas");
            ForwardChain();
            Console.WriteLine("3 DALIS. Rezultatai");
            if(completed)
            {
                Console.WriteLine("\t1) Tikslas " + result + " išvestas.");
                Console.Write("\t2) Kelias: ");
                if (usedRules.Count == 0)
                {
                    Console.WriteLine("Tikslas " + result + " tarp faktų. Kelias tuščias.");
                }
                else
                {
                    foreach (var rule in usedRules)
                    {
                        Console.Write(rule.name + ", ");
                    }
                    Console.WriteLine("\b\b.");

                }
            }
            else
            {
                Console.WriteLine("\t1) Tikslas " + result + " nerastas.");
            }
            
            Console.ReadLine();
        }
        
        public void PrintExecution(Rule rule)
        {
            switch(state)
            {
                case RuleState.UseRaiseFlag1:
                    Console.Write("\t\t"+rule + " taikome. Pakeliame flag1. Faktai ");
                    foreach(var fact in oldFacts)
                    {
                        Console.Write(fact + ", ");
                    }
                    Console.Write("\b\b ir ");
                    foreach(var fact in newFacts)
                    {
                        Console.Write(fact + ", ");
                    }
                    Console.WriteLine("\b\b.");
                    break;
                case RuleState.Flag1:
                    Console.WriteLine("\t\t" + rule + " praleidžiame, nes pakelta flag1(taisyklė panaudota).");
                    break;
                case RuleState.Flag2:
                    Console.WriteLine("\t\t" + rule + " netaikome, nes konsekventas faktuose. Pakeliame flag2.");
                    break;
                case RuleState.SkipNoFact:
                    Console.Write("\t\t" + rule + " netaikome ,nes trūksta ");
                    foreach(var requirement in rule.requirements)
                    {
                        if(!facts.Contains(requirement))
                        {
                            Console.WriteLine(requirement+".");
                            break;
                        }
                    }
                    break;
                case RuleState.SkipRaiseFlag2:
                    Console.WriteLine("\t\t" + rule + " praleidžiame, nes pakelta flag2.");
                    break;
            }
        }

        public void PrintData()
        {
            Console.WriteLine("1 DALIS. Duomenys");
            Console.WriteLine();
            Console.WriteLine("\t1) Taisyklės");
            foreach(var rule in rules)
            {
                Console.WriteLine("\t\t" + rule);
            }
            Console.WriteLine();
            Console.WriteLine("\t2) Faktai");
            Console.Write("\t\t");
            for(int i = 0; i < facts.Count; i++)
            {
                if(i != facts.Count -1 )
                {
                    Console.Write(facts[i]+ ", ");
                }
                else
                {
                    Console.Write(facts[i]);
                }
            }
            Console.Write("\b\b");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t3) Tikslas");
            Console.WriteLine("\t\t" + result);
        }

        public void ForwardChain()
        {
            while (progress && !completed)
            {
                if (facts.Contains(result))
                {
                    completed = true;
                    Console.WriteLine("\t\tTikslas gautas.");
                    Console.WriteLine();
                    break;
                }
                progress = false;
                Console.WriteLine();
                Console.WriteLine("\t"+iteration + " ITERACIJA");
                foreach(var rule in rules)
                {
                    if(rule.flag == Rule.Flag.flag1)
                    {
                        state = RuleState.Flag1;
                    }
                    else if(rule.flag == Rule.Flag.flag2)
                    {
                        state = RuleState.SkipRaiseFlag2;
                    }
                    else
                    {
                        if (facts.Contains(rule.result))
                        {
                            state = RuleState.Flag2;
                            rule.flag = Rule.Flag.flag2;
                        }
                        else
                        {
                            if(rule.requirements.All(element => facts.Contains(element)))
                            {
                                state = RuleState.UseRaiseFlag1;
                                facts.Add(rule.result);
                                newFacts.Add(rule.result);
                                usedRules.Add(rule);
                                rule.flag = Rule.Flag.flag1;
                                PrintExecution(rule);
                                progress = true;
                                break;
                            }
                            else
                            {
                                state = RuleState.SkipNoFact;
                            }
                        }
                    }
                    PrintExecution(rule);  
                }
                iteration++;
            }
        }

      

        public void ParseInput(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Julius\Desktop\University\Artificial Inteligence\ForwardChaining\ForwardChaining"+@"\"+fileName);
            State state = State.Default;
            foreach (var line in lines)
            {
                if (line == "1) Taisyklės")
                {
                    state = State.Rule;
                    continue;
                }

                if (line == "2) Faktai")
                {
                    state = State.Fact;
                    continue;
                }

                if (line == "3) Tikslas")
                {
                    state = State.Objective;
                    continue;
                }

                if (line == "")
                {
                    state = State.Default;
                    continue;
                }

                if (state == State.Rule)
                {
                    string[] facts = line.Split(' ');
                    List<Fact> factsForRule = new List<Fact>();
                    Fact result = null;

                    for (int i = 0; i < facts.Length;i++)
                    {
                        if(i == 0)
                        {
                            result = new Fact(facts[i]);
                        } else
                        {
                            factsForRule.Add(new Fact(facts[i]));
                        }
                    }
                    rules.Add(new Rule(factsForRule,result));
                }

                if (state == State.Fact)
                {
                    string[] strFacts = line.Split(' ');

                    for (int i = 0; i < strFacts.Length; i++)
                    {
                        facts.Add(new Fact(strFacts[i]));
                        oldFacts.Add(new Fact(strFacts[i]));
                    }
                }

                if(state == State.Objective)
                {
                    result = new Fact(line);
                }
            }

        }
    }
}

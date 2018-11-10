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
        int iteration = 0;
        enum State { Default, Rule, Fact, Objective}
        public enum RuleState {Default, Use,UseRaiseFlag1,SkipNoFact,SkipRaiseFlag2,Flag1,Flag2}
        RuleState state = RuleState.Default;
        bool progress = true;
        bool completed = false;



        public void Start()
        {
            ParseInput("input.txt");
            ForwardChain();
            

        }

        public void ForwardChain()
        {
            while (progress || !completed)
            {
                progress = false;

                foreach(var rule in rules)
                {
                    if(rule.flag == Rule.Flag.flag1)
                    {
                        state = RuleState.Flag1;
                        continue;
                    }
                    else if(rule.flag == Rule.Flag.flag2)
                    {
                        state = RuleState.Flag2;
                        continue;
                    }
                    else
                    {
                        if (facts.Contains(rule.result))
                        {
                            state = RuleState.SkipRaiseFlag2;
                            rule.flag = Rule.Flag.flag2;
                            continue;
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
                                break;
                            }
                            else
                            {
                                state = RuleState.SkipNoFact;
                                continue;
                            }
                        }
                    }
                    
                }
                if (facts.Contains(result))
                {
                    completed = true;
                }
                iteration++;
            }

            foreach(var rule in usedRules)
            {
                Console.WriteLine(rule.name);
            }
            Console.ReadLine();
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

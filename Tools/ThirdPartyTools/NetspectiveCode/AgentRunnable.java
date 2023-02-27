package com.covidien;

import java.io.IOException;

/**
 * Runnable Laptop Agent Interface.
 * @author Prakash
 *
 */
public class AgentRunnable implements Runnable {
	
	/**
	 * Thread.
	 */
	private static volatile Thread agentRunnable;
	/**
	 * Actual Laptop Agent static object.
	 */
	private static LaptopAgent agent = new LaptopAgent();
	/**
	 * Main thread start the laptop agent.
	 * @param args .
	 */
	public static void main(final String[] args) {
		
		if(args.length > 1) {
			usage();
		}
		
		if("start".equals(args[0])) {
			start();
		} else if("stop".equals(args[0])) {
			try {
				stop();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	/**
	 * Stopping Laptop Agent from service side.
	 * @throws IOException .
	 */
	private static void stop() throws IOException {
		if(agentRunnable!=null) {
			if(agent!=null) {
				agent.stop();
			}
			agentRunnable.interrupt();
		}
	}
	/**
	 * Startubg Laptop Agent from service side.
	 */
	private static void start() {
		agentRunnable = new Thread(new AgentRunnable());
		agentRunnable.start();
		
	}
	/**
	 * Error Message.
	 */
	private static void usage() {
		System.err.println("Must pass 'start' or 'stop'");
	}
	/**
	 * Run abstract function extension from Runnable interface.
	 */
	@Override
	public final void run() {
		try {
			agent.start();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

}
